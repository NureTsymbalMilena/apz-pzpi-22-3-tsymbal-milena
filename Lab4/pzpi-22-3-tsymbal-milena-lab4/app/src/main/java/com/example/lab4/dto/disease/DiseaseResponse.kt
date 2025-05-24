package com.example.lab4.dto.disease

data class DiseaseResponse(
    val diseaseId: String,
    val name: String,
    val severityLevel: String,
    val contagious: Boolean,
    val contagionRate: Double,
    val incubationPeriod: Int,
    val mortalityRate: Double,
    val transmissionMode: String
)
