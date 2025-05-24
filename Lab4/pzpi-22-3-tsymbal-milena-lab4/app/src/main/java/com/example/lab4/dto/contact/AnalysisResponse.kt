package com.example.lab4.dto.contact

data class AnalysisResponse(
    val name: String,
    val surname: String,
    val disease: String,
    val isContagious: Boolean,
    val totalRisk: Double,
    val riskLevel: String,
    val averageContactDuration: String,
    val contacts: List<Contact>,
    val potentialDiseases: List<String>
)

data class Contact(
    val userName: String,
    val userSurname: String,
    val userDisease: String,
    val roomName: String,
    val minDistance: Int,
    val contactStartTime: String,
    val contactEndTime: String
)
