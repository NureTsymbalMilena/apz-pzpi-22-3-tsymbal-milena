package com.example.lab4.ui.profile

import android.content.Context
import android.content.Context.MODE_PRIVATE
import android.content.Intent
import android.graphics.Typeface
import android.os.Bundle
import android.text.SpannableString
import android.text.Spanned
import android.text.style.StyleSpan
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.navigation.findNavController
import com.example.lab4.LoginActivity
import com.example.lab4.R
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.FragmentProfileBinding
import com.example.lab4.dto.disease.DiseaseResponse
import com.example.lab4.dto.hospital.HospitalResponse
import com.example.lab4.dto.user.UserResponse
import com.example.lab4.ui.editProfile.EditProfileFragment
import es.dmoral.toasty.Toasty
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.text.SimpleDateFormat
import java.util.Locale
import java.util.TimeZone

class ProfileFragment : Fragment() {

    private var _binding: FragmentProfileBinding? = null
    private val binding get() = _binding!!
    private var userResponse: UserResponse? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        _binding = FragmentProfileBinding.inflate(inflater, container, false)
        fetchUser()
        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun fetchUser() {
        val userApi = RetrofitClient.getInstance().userApi
        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val userId = sharedPref.getString("userId", null)

        if (userId == null) {
            Toasty.error(requireContext(), getString(R.string.user_id_is_missing), Toast.LENGTH_SHORT, true).show()
            return
        }

        userApi.getUser(userId).enqueue(object :
            Callback<UserResponse> {
            override fun onResponse(call: Call<UserResponse>, response: Response<UserResponse>) {
                if (response.isSuccessful) {
                    userResponse = response.body()
                    Log.d("Profile", "Received user data: $userResponse")
                    fetchDisease()

                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Profile", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<UserResponse>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Profile", "Failed: ${t.message}")
            }
        })
    }

    private fun deleteUser() {
        val userApi = RetrofitClient.getInstance().userApi

        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val userId = sharedPref.getString("userId", null)

        if (userId == null) {
            Toasty.error(requireContext(), getString(R.string.user_id_is_missing), Toast.LENGTH_SHORT, true).show()
            return
        }

        userApi.deleteUser(userId).enqueue(object :
            Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    Toasty.success(requireContext(),
                        getString(R.string.user_deleted_successfully), Toast.LENGTH_SHORT, true).show()
                    Log.d("Profile", "User successfully deleted: $userResponse")
                    fetchDisease()

                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Profile", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Profile", "Failed: ${t.message}")
            }
        })
    }

    private fun fetchDisease() {
        val diseaseId = userResponse?.diseaseId
        if (diseaseId == null) {
            Log.w("Profile", "Disease ID is null, skipping disease fetch")
            fetchHospital()
            return
        }

        val diseaseApi = RetrofitClient.getInstance().diseaseApi

        diseaseApi.getDisease(userResponse?.diseaseId).enqueue(object :
            Callback<DiseaseResponse> {
            override fun onResponse(call: Call<DiseaseResponse>, response: Response<DiseaseResponse>) {
                if (response.isSuccessful) {
                    userResponse?.disease = response.body()
                    Log.d("Disease", "Received user data: $userResponse")
                    fetchHospital()
                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Profile", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<DiseaseResponse>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Profile", "Failed: ${t.message}")
            }
        })
    }

    private fun fetchHospital() {
        val hospitalApi = RetrofitClient.getInstance().hospitalApi

        hospitalApi.getHospital(userResponse?.hospitalId).enqueue(object : Callback<HospitalResponse> {
            override fun onResponse(call: Call<HospitalResponse>, response: Response<HospitalResponse>) {
                if (response.isSuccessful) {
                    userResponse?.hospital = response.body()
                    Log.d("Hospitals", "Response: ${response}")
                    Log.d("Hospitals", "Response: ${response.body()}")
                    setUpFragment()
                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Hospitals", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<HospitalResponse>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Hospitals", "Failed: ${t.message}")
            }
        })
    }

    private fun setUpFragment() {
        val user = userResponse ?: return

        val nameLabel = getString(R.string.name)
        val fullName = "$nameLabel ${user.name} ${user.surname}"
        val spannableName = SpannableString(fullName).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, nameLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileName.text = spannableName

        val emailLabel = getString(R.string.email)
        val emailText = "$emailLabel ${user.email}"
        val spannableEmail = SpannableString(emailText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, emailLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileEmail.text = spannableEmail

        val regDateLabel = getString(R.string.registration_date)
        val formattedDate = formatDateTimeBasedOnLocale(user.createdAt, requireContext())
        val regDateText = "$regDateLabel $formattedDate"
        val spannableRegDate = SpannableString(regDateText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, regDateLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileRegistrationDate.text = spannableRegDate

        val roleLabel = getString(R.string.role)
        val roleText = "$roleLabel ${user.role}"
        val spannableRole = SpannableString(roleText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, roleLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileRole.text = spannableRole

        val hospitalLabel = getString(R.string.hospital_name)
        val hospitalText = "$hospitalLabel ${user.hospital?.name ?: getString(R.string.unknown)}"
        val spannableHospital = SpannableString(hospitalText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, hospitalLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileHospitalName.text = spannableHospital

        val diseaseLabel = getString(R.string.disease)
        val diseaseText = "$diseaseLabel ${user.disease?.name ?: getString(R.string.none)}"
        val spannableDisease = SpannableString(diseaseText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, diseaseLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileDisease.text = spannableDisease

        val contagiousLabel = getString(R.string.is_contagious)
        val contagiousStatus = if (user.disease?.contagious == true) getString(R.string.yes) else getString(R.string.no)
        val contagiousText = "$contagiousLabel $contagiousStatus"
        val spannableContagious = SpannableString(contagiousText).apply {
            setSpan(StyleSpan(Typeface.BOLD), 0, contagiousLabel.length, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        }
        binding.profileIsContagious.text = spannableContagious

        binding.editButton.setOnClickListener {
            val user = userResponse ?: return@setOnClickListener

            val bundle = Bundle().apply {
                putString("userId", user.userId ?: "")
                putString("name", user.name ?: "")
                putString("surname", user.surname ?: "")
                putString("email", user.email ?: "")
            }

            val navController = requireActivity().findNavController(R.id.nav_host_fragment_activity_main)
            navController.navigate(R.id.navigation_editProfile, bundle)
        }

        binding.deleteButton.setOnClickListener {
            deleteUser()
        }

        binding.logoutButton.setOnClickListener {
            val sharedPref = requireContext().getSharedPreferences("auth_prefs", MODE_PRIVATE)
            with(sharedPref.edit()) {
                clear()
                apply()
            }

            val intent = Intent(requireContext(), LoginActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
            startActivity(intent)
            requireActivity().finish()
        }
    }

    fun formatDateTimeBasedOnLocale(isoDateTime: String, context: Context): String {
        return try {
            val inputFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
            inputFormat.timeZone = TimeZone.getTimeZone("UTC")

            val date = inputFormat.parse(isoDateTime)

            val locale = context.resources.configuration.locales.get(0)
            val outputFormat = when (locale.language) {
                "uk" -> SimpleDateFormat("dd.MM.yyyy HH:mm", locale)
                "en" -> SimpleDateFormat("MM/dd/yyyy hh:mm a", locale)
                else -> SimpleDateFormat("yyyy-MM-dd HH:mm", locale)
            }

            outputFormat.format(date!!)
        } catch (e: Exception) {
            isoDateTime
        }
    }

}