package com.example.lab4.ui.editProfile

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.navigation.findNavController
import androidx.navigation.fragment.findNavController
import com.example.lab4.R
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.FragmentEditProfileBinding
import com.example.lab4.databinding.FragmentProfileBinding
import com.example.lab4.dto.hospital.HospitalResponse
import com.example.lab4.dto.user.UserDto
import com.example.lab4.dto.user.UserResponse
import es.dmoral.toasty.Toasty
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class EditProfileFragment : Fragment() {

    private var _binding: FragmentEditProfileBinding? = null
    private val binding get() = _binding!!

    private var userId: String? = null

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        userId = arguments?.getString("userId") ?: ""
        val name = arguments?.getString("name") ?: ""
        val surname = arguments?.getString("surname") ?: ""
        val email = arguments?.getString("email") ?: ""

        binding.editNameInput.setText(name)
        binding.editSurnameInput.setText(surname)
        binding.editEmailInput.setText(email)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentEditProfileBinding.inflate(inflater, container, false)

        binding.editButton.setOnClickListener {
            val userIdNotNull = userId
            if (userIdNotNull.isNullOrEmpty()) {
                Toast.makeText(requireContext(),
                    getString(R.string.user_id_is_missing), Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val userDto = UserDto(
                name = binding.editNameInput.text.toString(),
                surname = binding.editSurnameInput.text.toString(),
                email = binding.editEmailInput.text.toString()
            )
            updateUserData(userIdNotNull, userDto)
        }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    fun updateUserData(userId: String, userDto: UserDto) {
        val userApi = RetrofitClient.getInstance().userApi

        userApi.updateUser(userId, userDto).enqueue(object : Callback<UserResponse> {
            override fun onResponse(call: Call<UserResponse>, response: Response<UserResponse>) {
                if (response.isSuccessful) {
                    val updatedUser = response.body()
                    Toasty.success(requireContext(),
                        getString(R.string.user_updated_successfully), Toast.LENGTH_SHORT, true).show()
                    Log.d("UpdateUser", "Response: $updatedUser")

                    val navController = requireActivity().findNavController(R.id.nav_host_fragment_activity_main)
                    navController.navigate(R.id.navigation_profile)
                } else {
                    Toasty.error(requireContext(),
                        getString(R.string.update_failed), Toast.LENGTH_SHORT, true).show()
                    Log.e("UpdateUser", "Response failed: $response")
                }
            }

            override fun onFailure(call: Call<UserResponse>, t: Throwable) {
                Toasty.error(requireContext(), "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("UpdateUser", "Failed: ${t.message}")
            }
        })
    }

}