package com.example.lab4.api

import com.example.lab4.dto.admin.ChangeRoleDto
import com.example.lab4.dto.auth.LoginDto
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.PATCH
import retrofit2.http.POST

interface AdminApi {

    @PATCH("Admin/role")
    fun changeRole(@Header("Authorization") accessToken: String, @Body changeRoleDto: ChangeRoleDto): Call<ResponseBody>

    @POST("Admin/backup")
    fun backup(@Header("Authorization") accessToken: String, @Body outputDirectory: String): Call<ResponseBody>

    @POST("Admin/restore")
    fun restore(
        @Header("Authorization") token: String,
        @Body backupFilePath: String
    ): Call<ResponseBody>
}