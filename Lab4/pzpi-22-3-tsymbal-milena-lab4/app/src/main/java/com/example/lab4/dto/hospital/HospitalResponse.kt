package com.example.lab4.dto.hospital

data class HospitalResponse(
    val hospitalId: String,
    val name: String,
    val address: String,
    val x: Int,
    val y: Int,
    val z: Int
)

