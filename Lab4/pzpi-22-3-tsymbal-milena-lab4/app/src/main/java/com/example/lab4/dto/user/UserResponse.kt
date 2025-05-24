package com.example.lab4.dto.user

import com.example.lab4.dto.disease.DiseaseResponse
import com.example.lab4.dto.hospital.HospitalResponse

data class UserResponse(
    val userId: String,
    val hospitalId: String,
    var hospital: HospitalResponse?,
    val deviceId: String?,
    val device: Any?,
    val name: String,
    val surname: String,
    val email: String,
    val createdAt: String,
    val role: String,
    val diseaseId: String,
    var disease: DiseaseResponse?,
    val password: String,
    val x: Double,
    val y: Double,
    val z: Double
)
