package com.example.lab4

import android.content.Intent
import android.graphics.Color
import android.os.Bundle
import android.text.SpannableString
import android.text.Spanned
import android.text.method.LinkMovementMethod
import android.text.style.ClickableSpan
import android.text.style.ForegroundColorSpan
import android.text.style.UnderlineSpan
import android.util.Log
import android.view.View
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.ActivityLoginBinding
import com.example.lab4.dto.auth.LoginDto
import com.example.lab4.dto.auth.LoginResponse
import es.dmoral.toasty.Toasty
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class LoginActivity : AppCompatActivity() {
    private lateinit var _binding: ActivityLoginBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        supportActionBar?.hide()
        enableEdgeToEdge()
        _binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(_binding.root)
        ViewCompat.setOnApplyWindowInsetsListener(_binding.loginContainer) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        _binding.loginButton.setOnClickListener {
            login()
        }

        val registerTextView = _binding.registerTextView
        val fullText = getString(R.string.don_t_have_an_account_register)
        val spannable = SpannableString(fullText)

        val clickableSpan = object : ClickableSpan() {
            override fun onClick(widget: View) {
                val intent = Intent(this@LoginActivity, RegisterActivity::class.java)
                startActivity(intent)
            }
        }

        val start = fullText.indexOf(getString(R.string.register))
        val end = start + getString(R.string.register).length

        val grayColor = ContextCompat.getColor(this, android.R.color.darker_gray)

        spannable.setSpan(clickableSpan, start, end, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        spannable.setSpan(UnderlineSpan(), start, end, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)
        spannable.setSpan(ForegroundColorSpan(grayColor), start, end, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE)

        registerTextView.text = spannable
        registerTextView.movementMethod = LinkMovementMethod.getInstance()
        registerTextView.highlightColor = Color.TRANSPARENT
    }

    fun login(){
        val loginDto = LoginDto(
            email = _binding.loginEmailInput.text.toString(),
            password = _binding.loginPasswordInput.text.toString(),
        )

        val authApi = RetrofitClient.getInstance().authApi

        authApi.login(loginDto).enqueue(object :
            Callback<LoginResponse> {
            override fun onResponse(call: Call<LoginResponse>, response: Response<LoginResponse>) {
                if (response.isSuccessful) {
                    val loginResponse = response.body()

                    val sharedPref = getSharedPreferences("auth_prefs", MODE_PRIVATE)
                    with(sharedPref.edit()) {
                        putString("accessToken", loginResponse?.accessToken)
                        putString("refreshToken", loginResponse?.refreshToken)
                        putString("userId", loginResponse?.userId)
                        putString("role", loginResponse?.role)
                        apply()
                    }

                    Toasty.success(this@LoginActivity,
                        getString(R.string.you_logged_in_successfully), Toast.LENGTH_SHORT, true).show()
                    Log.d("Login", "User successfully logged in: $loginResponse")

                    val intent = Intent(this@LoginActivity, MainActivity::class.java)
                    startActivity(intent)
                    finish()
                } else {
                    Toasty.error(this@LoginActivity, "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Login", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                Toasty.error(this@LoginActivity, "${getString(R.string.response_failed)}"
                    , Toast.LENGTH_SHORT, true).show()
                Log.e("Login", "Failed: ${t.message}")
            }
        })
    }
}