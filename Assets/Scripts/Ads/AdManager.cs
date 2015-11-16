using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Ads {
    public class AdManager : MonoBehaviour {

        private BannerView bannerView;
        public bool bannerAdReady;

        [System.NonSerialized]
        public static AdManager Instance;

        public AdManager() {
            Instance = this;
        }

        void Start() {
            if (GameObject.FindObjectsOfType<AdManager>().Length > 1) {
                Destroy(gameObject);
            }
            loadBanner();
        }

        void loadBanner() {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(
                    "ca-app-pub-3223918612095593/2835390268", AdSize.Banner, AdPosition.Bottom);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            bannerView.LoadAd(request);
            // Called when an ad request has successfully loaded.
            bannerView.AdLoaded += HandleAdLoaded;
            bannerView.AdFailedToLoad += HandleAdFailedToLoad;
            hideBannerAd();
        }

        public void HandleAdLoaded(object sender, EventArgs args) {
            Debug.Log("HandleAdLoaded event received.");
            bannerAdReady = true;
        }
        public void HandleAdFailedToLoad(object sender, EventArgs args) {
            Debug.Log("HandleAdFailedToLoaded event received.");
            bannerAdReady = false;
        }

        void Awake() {
            DontDestroyOnLoad(this);
        }

        public void showBannerAd() {
            Debug.Log("showBannerAd()");
            bannerView.Show();
        }

        public void hideBannerAd() {
            bannerView.Hide();
        }

        public IEnumerator ShowBannerAdWhenReady() {
            while (!bannerAdReady) {
                yield return null;
            }
            showBannerAd();
        }
    }
}