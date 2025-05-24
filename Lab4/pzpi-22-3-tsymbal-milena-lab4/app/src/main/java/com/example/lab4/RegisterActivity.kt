package com.example.lab4

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.lab4.LoginActivity
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.ActivityLoginBinding
import com.example.lab4.databinding.ActivityRegisterBinding
import com.example.lab4.dto.auth.LoginDto
import com.example.lab4.dto.auth.LoginResponse
import com.example.lab4.dto.auth.RegisterDto
import com.example.lab4.dto.hospital.HospitalResponse
import es.dmoral.toasty.Toasty
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RegisterActivity : AppCompatActivity() {

    private lateinit var _binding: ActivityRegisterBinding
    private lateinit var hospitalsResponse: List<HospitalResponse>

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        supportActionBar?.hide()
        enableEdgeToEdge()
        _binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(_binding.root)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.registerContainer)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        fetchHospitals()

        _binding.registerButton.setOnClickListener {
            register()
        }
    }

    private fun register() {
        val selectedIndex = _binding.registerHospitalNameSelect.selectedItemPosition
        if (selectedIndex <= 0) {
            Toasty.error(this, "Please select a hospital", Toast.LENGTH_SHORT, true).show()
            return
        }

        val registerDto = RegisterDto(
            name = _binding.registerNameInput.text.toString(),
            surname = _binding.registerSurnameInput.text.toString(),
            email = _binding.registerEmailInput.text.toString(),
            password = _binding.registerPasswordInput.text.toString(),
            hospitalName = _binding.registerHospitalNameSelect.selectedItem.toString(),
        )

        val authApi = RetrofitClient.getInstance().authApi

        authApi.register(registerDto).enqueue(object :
            Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    val registerResponse = response.body()

                    Toasty.success(
                        this@RegisterActivity,
                        "You registered successfully!",
                        Toast.LENGTH_SHORT,
                        true
                    ).show()
                    Log.d("Register", "User successfully logged in: $registerResponse")

                    val intent = Intent(this@RegisterActivity, LoginActivity::class.java)
                    startActivity(intent)
                    finish()

                } else {
                    Toasty.error(
                        this@RegisterActivity,
                        "Response failed: ${response}",
                        Toast.LENGTH_SHORT,
                        true
                    ).show()
                    Log.e("Register", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toasty.error(
                    this@RegisterActivity,
                    "Response failed with code: ${t.message}",
                    Toast.LENGTH_SHORT,
                    true
                ).show()
                Log.e("Register", "Failed: ${t.message}")
            }
        })
    }

    private fun fetchHospitals() {
        val hospitalApi = RetrofitClient.getInstance().hospitalApi

        hospitalApi.getAllHospitals().enqueue(object : Callback<List<HospitalResponse>> {
            override fun onResponse(call: Call<List<HospitalResponse>>, response: Response<List<HospitalResponse>>) {
                if (response.isSuccessful) {
                    hospitalsResponse = response.body() ?: emptyList()
                    Log.d("Hospitals", "Loaded: ${hospitalsResponse.size} items")

                    val hospitalsNames = prepareSpinnerList(hospitalsResponse,
                        getString(R.string.select_hospital)) { it.name }

                    val adapter = ArrayAdapter(
                        this@RegisterActivity,
                        android.R.layout.simple_spinner_item,
                        hospitalsNames
                    )
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                    _binding.registerHospitalNameSelect.adapter = adapter
                } else {
                    Toasty.error(this@RegisterActivity,
                        getString(R.string.failed_to_load_hospitals), Toast.LENGTH_SHORT, true).show()
                }
            }

            override fun onFailure(call: Call<List<HospitalResponse>>, t: Throwable) {
                Toasty.error(this@RegisterActivity, "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Hospitals", "Failure: ${t.message}")
            }
        })
    }

    private fun <T> prepareSpinnerList(
        items: List<T>,
        placeholder: String,
        displayFunc: (T) -> String
    ): List<String> {
        val list = mutableListOf(placeholder)
        list.addAll(items.map(displayFunc))
        return list
    }
}