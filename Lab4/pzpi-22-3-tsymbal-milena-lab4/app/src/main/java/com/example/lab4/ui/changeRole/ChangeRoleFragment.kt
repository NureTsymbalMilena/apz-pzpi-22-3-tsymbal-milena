package com.example.lab4.ui.changeRole

import android.content.Context
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.lab4.R
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.FragmentChangeRoleBinding
import com.example.lab4.dto.admin.ChangeRoleDto
import com.example.lab4.dto.contact.AnalysisResponse
import com.example.lab4.dto.hospital.HospitalResponse
import com.example.lab4.dto.user.UserResponse
import es.dmoral.toasty.Toasty
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class ChangeRoleFragment : Fragment() {

    private var _binding: FragmentChangeRoleBinding? = null
    private val binding get() = _binding!!

    private var usersResponse: List<UserResponse>? = null
    private val roles = listOf(
        "User",
        "PlatformAdmin",
        "SystemAdmin",
        "DatabaseAdmin",
        "HospitalAdmin"
    )

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentChangeRoleBinding.inflate(inflater, container, false)

        binding.changeButton.setOnClickListener {
            changeRole()
        }

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        fetchUsers()
        setupRoleSpinner()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun <T> prepareSpinnerList(
        items: List<T>,
        placeholder: String,
        displayFunc: (T) -> String
    ): List<String> {
        val list = mutableListOf<String>()
        list.add(placeholder)
        list.addAll(items.map { displayFunc(it) })
        return list
    }

    private fun fetchUsers() {
        val userApi = RetrofitClient.getInstance().userApi

        userApi.getAllUsers().enqueue(object : Callback<List<UserResponse>> {
            override fun onResponse(call: Call<List<UserResponse>>, response: Response<List<UserResponse>>) {
                if (response.isSuccessful) {
                    usersResponse = response.body()

                    val userNames = prepareSpinnerList(usersResponse ?: emptyList(),
                        getString(R.string.select_user_email)) { user ->
                        "${user.email}"
                    }

                    val adapter = android.widget.ArrayAdapter(
                        requireContext(),
                        android.R.layout.simple_spinner_item,
                        userNames
                    )
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                    binding.changeEmailSpinner.adapter = adapter

                    Log.e("ChangeRole", "Users fetched successfully: ${response}")
                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("ChangeRole", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<List<UserResponse>>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("ChangeRole", "Failed: ${t.message}")
            }
        })
    }

    private fun setupRoleSpinner() {
        val rolesWithPlaceholder = mutableListOf(getString(R.string.select_role))
        rolesWithPlaceholder.addAll(roles)

        val adapter = android.widget.ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            rolesWithPlaceholder
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        binding.changeRoleSpinner.adapter = adapter
    }

    private fun changeRole(){
        val adminApi = RetrofitClient.getInstance().adminApi

        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val accessToken = sharedPref.getString("accessToken", null)

        if (accessToken == null) {
            Toasty.error(requireContext(), getString(R.string.access_token_not_found), Toast.LENGTH_SHORT, true).show()
            return
        }
        val bearerToken = "Bearer $accessToken"
        val email = binding.changeEmailSpinner.selectedItem?.toString()
        val role = binding.changeRoleSpinner.selectedItem?.toString()

        if (email.isNullOrBlank() || role.isNullOrBlank()
            || email == getString(R.string.select_user_email)
            || role == getString(R.string.select_role)
        ) {
            Toasty.warning(requireContext(),
                getString(R.string.please_select_both_email_and_role), Toast.LENGTH_SHORT, true).show()
            return
        }

        val changeRoleDto = ChangeRoleDto(
            userEmail = email,
            role = role
        )
        Log.d("ChangeRole", "${email} ${role} ${bearerToken}")
        adminApi.changeRole(bearerToken, changeRoleDto).enqueue(object : Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    Toasty.success(requireContext(),
                        getString(R.string.role_changed_successfully), Toast.LENGTH_SHORT, true).show()
                    Log.d("ChangeRole", "Role changed successfully")
                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("ChangeRole", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("ChangeRole", "Failed: ${t.message}")
            }
        })
    }
}