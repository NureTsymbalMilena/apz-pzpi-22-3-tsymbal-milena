package com.example.lab4.ui.home

import android.content.Context
import android.content.Context.MODE_PRIVATE
import android.graphics.Typeface
import android.os.Bundle
import android.text.SpannableString
import android.text.Spanned
import android.text.style.StyleSpan
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.lab4.R
import com.example.lab4.api.RetrofitClient
import com.example.lab4.databinding.FragmentHomeBinding
import com.example.lab4.dto.contact.AnalysisResponse
import com.example.lab4.dto.hospital.HospitalResponse
import es.dmoral.toasty.Toasty
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.text.SimpleDateFormat
import java.util.Locale
import java.util.TimeZone

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!
    private var analysisResponse: AnalysisResponse? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        fetchAnalysis()

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun fetchAnalysis() {
        val contactApi = RetrofitClient.getInstance().contactApi
        val sharedPref = requireContext().getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
        val userId = sharedPref.getString("userId", null)

        if (userId == null) {
            Toasty.error(requireContext(), getString(R.string.user_id_is_missing), Toast.LENGTH_SHORT, true).show()
            return
        }

        contactApi.epidemiologicalRiskAnalysis(userId).enqueue(object : Callback<AnalysisResponse> {
            override fun onResponse(call: Call<AnalysisResponse>, response: Response<AnalysisResponse>) {
                if (response.isSuccessful) {
                    analysisResponse = response.body()
                    Log.d("Analysis", "Received analysis: $analysisResponse")
                    setUpFragment()
                } else {
                    Toasty.error(requireContext(),
                        getString(R.string.restore_failed), Toast.LENGTH_SHORT, true).show()
                    Log.e("Analysis", "Response failed: ${response}")
                }
            }

            override fun onFailure(call: Call<AnalysisResponse>, t: Throwable) {
                Toasty.error(requireContext(),
                    getString(R.string.restore_failed), Toast.LENGTH_SHORT, true).show()
                Log.e("Analysis", "Failed: ${t.message}")
            }
        })
    }

    private fun getSpannable(label: String, value: String): SpannableString {
        val fullText = "$label $value"
        val spannable = SpannableString(fullText)
        spannable.setSpan(
            StyleSpan(Typeface.BOLD),
            0,
            label.length,
            Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
        )
        return spannable
    }


    private fun setUpFragment() {
        analysisResponse?.let { analysis ->
            val nameLabel = getString(R.string.name)
            val fullNameText = "$nameLabel ${analysis.name} ${analysis.surname}"
            val spannableName = SpannableString(fullNameText)
            spannableName.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                nameLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeName.text = spannableName

            val diseaseLabel = getString(R.string.disease)
            val fullDiseaseText = "$diseaseLabel ${analysis.disease}"
            val spannableDisease = SpannableString(fullDiseaseText)
            spannableDisease.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                diseaseLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeDisease.text = spannableDisease

            val contagiousLabel = getString(R.string.is_contagious)
            val contagiousText = "$contagiousLabel ${if (analysis.isContagious) getString(R.string.yes) else getString(R.string.no)}"
            val spannableContagious = SpannableString(contagiousText)
            spannableContagious.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                contagiousLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeIsContagious.text = spannableContagious

            val totalRiskLabel = getString(R.string.total_risk)
            val totalRiskText = "$totalRiskLabel ${analysis.totalRisk}"
            val spannableTotalRisk = SpannableString(totalRiskText)
            spannableTotalRisk.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                totalRiskLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeTotalRisk.text = spannableTotalRisk

            val riskLevelLabel = getString(R.string.risk_level)
            val riskLevelText = "$riskLevelLabel ${analysis.riskLevel}"
            val spannableRiskLevel = SpannableString(riskLevelText)
            spannableRiskLevel.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                riskLevelLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeRiskLevel.text = spannableRiskLevel

            val rawDuration = analysis.averageContactDuration
            val formattedDuration = formatTimeStringToLocalizedDuration(rawDuration, requireContext())
            binding.homeAverageContactDuration.text = getSpannable(
                getString(R.string.average_contact_duration),
                formattedDuration
            )

            val potentialDiseasesLabel = getString(R.string.potential_diseases)
            val potentialDiseasesText = if (analysis.potentialDiseases.isNotEmpty())
                analysis.potentialDiseases.joinToString(", ")
            else
                getString(R.string.none)
            val fullPotentialDiseasesText = "$potentialDiseasesLabel $potentialDiseasesText"
            val spannablePotentialDiseases = SpannableString(fullPotentialDiseasesText)
            spannablePotentialDiseases.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                potentialDiseasesLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homePotentialDiseases.text = spannablePotentialDiseases

            val contactsLabel = getString(R.string.contacts_amount)
            val contactsText = "$contactsLabel ${analysis.contacts.size}"
            val spannableContacts = SpannableString(contactsText)
            spannableContacts.setSpan(
                StyleSpan(Typeface.BOLD),
                0,
                contactsLabel.length,
                Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
            )
            binding.homeContacts.text = spannableContacts

            val contactContainer = binding.contactContainer
            contactContainer.removeAllViews()

            val inflater = LayoutInflater.from(requireContext())

            analysis.contacts.forEach { contact ->
                val contactView = inflater.inflate(R.layout.item_contact, contactContainer, false)

                contactView.findViewById<TextView>(R.id.contactName).text =
                    getSpannable(getString(R.string.name), "${contact.userName} ${contact.userSurname}")

                contactView.findViewById<TextView>(R.id.contactDisease).text =
                    getSpannable(getString(R.string.disease), contact.userDisease)

                contactView.findViewById<TextView>(R.id.contactRoom).text =
                    getSpannable(getString(R.string.room_name), contact.roomName)

                contactView.findViewById<TextView>(R.id.contactDistance).text =
                    getSpannable(getString(R.string.minimal_distance), formatDistanceBasedOnLocale(contact.minDistance, requireContext()))

                val formattedStart = formatDateTimeBasedOnLocale(contact.contactStartTime, requireContext())
                contactView.findViewById<TextView>(R.id.contactStartTime).text =
                    getSpannable(getString(R.string.contact_start_time), formattedStart)

                val formattedEnd = formatDateTimeBasedOnLocale(contact.contactEndTime, requireContext())
                contactView.findViewById<TextView>(R.id.contactEndTime).text =
                    getSpannable(getString(R.string.contact_end_time), formattedEnd)

                contactContainer.addView(contactView)
            }
        }
    }

    fun formatDateTimeBasedOnLocale(isoDateTime: String, context: Context): String {
        return try {
            val inputFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault())
            inputFormat.timeZone = TimeZone.getTimeZone("UTC")

            val date = inputFormat.parse(isoDateTime)

            val locale = context.resources.configuration.locales.get(0)
            val outputFormat = when (locale.language) {
                "uk" -> SimpleDateFormat("dd.MM.yyyy HH:mm", locale)
                "en" -> SimpleDateFormat("MM/dd/yyyy hh:mm a", locale)
                else -> SimpleDateFormat("yyyy-MM-dd HH:mm", locale)
            }

            outputFormat.format(date!!)
        } catch (e: Exception) {
            isoDateTime
        }
    }

    fun formatTimeStringToLocalizedDuration(timeString: String, context: Context): String {
        val parts = timeString.split(":", ".", limit = 4)
        if (parts.size < 3) return timeString // fallback

        val hours = parts[0].toIntOrNull() ?: 0
        val minutes = parts[1].toIntOrNull() ?: 0
        val seconds = parts[2].toIntOrNull() ?: 0

        val locale = context.resources.configuration.locales[0]
        val isUkrainian = locale.language == "uk"

        val hLabel = if (isUkrainian) "год" else "h"
        val mLabel = if (isUkrainian) "хв" else "m"
        val sLabel = if (isUkrainian) "с" else "s"

        return buildString {
            if (hours > 0) append("$hours $hLabel ")
            if (minutes > 0) append("$minutes $mLabel ")
            append("$seconds $sLabel")
        }.trim()
    }

    private fun formatDistanceBasedOnLocale(meters: Int, context: Context): String {
        val locale = context.resources.configuration.locales.get(0)
        return when (locale.language) {
            "uk" -> String.format(Locale("uk"), "%d м", meters)
            else -> {
                val feet = meters * 3.28084
                String.format(Locale.ENGLISH, "%.2f ft", feet)
            }
        }
    }

}