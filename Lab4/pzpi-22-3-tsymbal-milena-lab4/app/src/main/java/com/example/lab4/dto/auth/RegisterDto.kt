package com.example.lab4.dto.auth

data class RegisterDto(
    val name: String,
    val surname: String,
    val email: String,
    val password: String,
    val hospitalName: String,
)
