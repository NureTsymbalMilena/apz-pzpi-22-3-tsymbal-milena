package com.example.lab4.dto.auth

data class LoginResponse(
    val accessToken: String,
    val refreshToken: String,
    val userId: String,
    val role: String
)
