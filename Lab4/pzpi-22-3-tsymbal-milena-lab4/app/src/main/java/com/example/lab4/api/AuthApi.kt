package com.example.lab4.api

import com.example.lab4.dto.auth.LoginDto
import com.example.lab4.dto.auth.LoginResponse
import com.example.lab4.dto.auth.RegisterDto
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthApi {

    @POST("Auth/register")
    fun register(@Body registerDto: RegisterDto): Call<ResponseBody>

    @POST("Auth/login")
    fun login(@Body loginDto: LoginDto): Call<LoginResponse>
}