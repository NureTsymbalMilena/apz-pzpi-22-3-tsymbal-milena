package com.example.lab4.api

import com.example.lab4.dto.contact.AnalysisResponse
import com.example.lab4.dto.user.UserDto
import com.example.lab4.dto.user.UserResponse
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.PATCH
import retrofit2.http.POST
import retrofit2.http.Path

interface UserApi {

    @GET("User/{userId}")
    fun getUser(@Path("userId") userId: String): Call<UserResponse>

    @PATCH("User/{userId}")
    fun updateUser(
        @Path("userId") userId: String,
        @Body userDto: UserDto
    ): Call<UserResponse>

    @DELETE("User/{userId}")
    fun deleteUser(@Path("userId") userId: String): Call<ResponseBody>

    @GET("User")
    fun getAllUsers(): Call<List<UserResponse>>

}