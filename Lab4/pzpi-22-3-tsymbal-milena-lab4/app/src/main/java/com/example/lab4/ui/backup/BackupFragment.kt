package com.example.lab4.ui.backup

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.os.Environment
import android.provider.DocumentsContract
import android.provider.MediaStore
import android.provider.OpenableColumns
import android.util.Log
import androidx.activity.result.ActivityResultLauncher
import androidx.activity.result.contract.ActivityResultContracts
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.text.HtmlCompat
import com.example.lab4.R
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.FragmentBackupBinding
import es.dmoral.toasty.Toasty
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.io.File
import java.io.IOException
import okhttp3.MultipartBody
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.FileInputStream
import java.io.FileOutputStream
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

class BackupFragment : Fragment() {

    private var _binding: FragmentBackupBinding? = null
    private val binding get() = _binding!!

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentBackupBinding.inflate(inflater, container, false)

        binding.backupButton.setOnClickListener {
            if (binding.backupInput.text == null) {
                Toasty.warning(requireContext(),
                    getString(R.string.write_backup_directory), Toast.LENGTH_SHORT, true).show()
            } else {
                backup()
            }
        }

        binding.restoreButton.setOnClickListener {
            if (binding.restoreInput.text == null) {
                Toasty.warning(requireContext(),
                    getString(R.string.write_restore_file_path), Toast.LENGTH_SHORT, true).show()
            } else {
                restore()
            }
        }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun backup(){
        val adminApi = RetrofitClient.getInstance().adminApi

        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val accessToken = sharedPref.getString("accessToken", null)

        if (accessToken == null) {
            Toasty.error(requireContext(),
                getString(R.string.access_token_not_found), Toast.LENGTH_SHORT, true).show()
            return
        }
        val bearerToken = "Bearer $accessToken"

        adminApi.backup(bearerToken, binding.backupInput.text.toString()).enqueue(object : Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    Toasty.success(requireContext(),
                        getString(R.string.backup_made_successfully), Toast.LENGTH_SHORT, true).show()
                    Log.d("Backup", getString(R.string.backup_made_successfully))
                } else {
                    Toasty.error(requireContext(), "${getString(R.string.response_failed)} ${response.code()}"
                        , Toast.LENGTH_SHORT, true).show()
                    Log.e("Backup", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toasty.error(requireContext(),
                    getString(R.string.network_error, t.message), Toast.LENGTH_SHORT, true).show()
                Log.e("Backup", "Failed: ${t.message}")
            }
        })
    }

    private fun restore() {
        val adminApi = RetrofitClient.getInstance().adminApi

        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val accessToken = sharedPref.getString("accessToken", null)

        if (accessToken == null) {
            Toasty.error(requireContext(), getString(R.string.access_token_not_found), Toast.LENGTH_SHORT, true).show()
            return
        }
        val bearerToken = "Bearer $accessToken"

        adminApi.restore(bearerToken, binding.restoreInput.text.toString()).enqueue(object : Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                if (response.isSuccessful) {
                    Toasty.success(requireContext(),
                        getString(R.string.restore_successful), Toast.LENGTH_SHORT, true).show()
                    Log.d("Restore", "Restore successful")
                } else {
                    Toasty.error(requireContext(),
                        getString(R.string.restore_failed), Toast.LENGTH_SHORT, true).show()
                    Log.e("Restore", "Restore failed: ${response}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                Toasty.error(requireContext(), getString(R.string.network_error, t.message), Toast.LENGTH_SHORT, true).show()
                Log.e("Restore", "Failed: ${t.message}")
            }
        })
    }

}
