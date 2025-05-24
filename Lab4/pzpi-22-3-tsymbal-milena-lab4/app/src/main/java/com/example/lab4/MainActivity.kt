package com.example.lab4

import android.content.Context
import android.os.Bundle
import com.google.android.material.bottomnavigation.BottomNavigationView
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.example.lab4.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val navView: BottomNavigationView = binding.navView

        val navController = findNavController(R.id.nav_host_fragment_activity_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        val appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.navigation_home, R.id.navigation_changeRole, R.id.navigation_profile, R.id.navigation_backup
            )
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        val menu = navView.menu

        val sharedPref = this@MainActivity.getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val role = sharedPref.getString("role", null)

        when (role) {
            "User" -> {
                menu.findItem(R.id.navigation_home).isVisible = true
                menu.findItem(R.id.navigation_changeRole).isVisible = false
                menu.findItem(R.id.navigation_backup).isVisible = false
                menu.findItem(R.id.navigation_profile).isVisible = true
            }
            "PlatformAdmin" -> {
                menu.findItem(R.id.navigation_home).isVisible = true
                menu.findItem(R.id.navigation_changeRole).isVisible = true
                menu.findItem(R.id.navigation_backup).isVisible = false
                menu.findItem(R.id.navigation_profile).isVisible = true
            }
            "DatabaseAdmin" -> {
                menu.findItem(R.id.navigation_home).isVisible = true
                menu.findItem(R.id.navigation_changeRole).isVisible = false
                menu.findItem(R.id.navigation_backup).isVisible = true
                menu.findItem(R.id.navigation_profile).isVisible = true
            }
        }
    }
}