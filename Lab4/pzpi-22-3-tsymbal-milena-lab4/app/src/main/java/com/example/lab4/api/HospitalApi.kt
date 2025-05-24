package com.example.lab4.api

import com.example.lab4.dto.hospital.HospitalResponse
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path

interface HospitalApi {

    @GET("Hospital")
    fun getAllHospitals(): Call<List<HospitalResponse>>

    @GET("Hospital/{hospitalId}")
    fun getHospital(@Path("hospitalId") hospitalId: String?): Call<HospitalResponse>
}