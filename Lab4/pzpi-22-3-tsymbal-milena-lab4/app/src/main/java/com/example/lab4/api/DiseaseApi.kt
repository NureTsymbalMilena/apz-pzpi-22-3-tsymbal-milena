package com.example.lab4.api

import com.example.lab4.dto.disease.DiseaseResponse
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path

interface DiseaseApi {

    @GET("Disease/{diseaseId}")
    fun getDisease(@Path("diseaseId") diseaseId: String?): Call<DiseaseResponse>
}