package com.example.lab4.api

import com.example.lab4.dto.contact.AnalysisResponse
import retrofit2.Call
import retrofit2.http.POST
import retrofit2.http.Path

interface ContactApi {

    @POST("Contact/{userId}/epidemiological-risk-analysis")
    fun epidemiologicalRiskAnalysis(@Path("userId") userId: String): Call<AnalysisResponse>
}