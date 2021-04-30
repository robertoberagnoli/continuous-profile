//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

[System.Serializable]
public class Tweet
{
    public string Text { get; set; }
}


namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;


   
    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;
        public Texture2D texture;
        public int counter = 0;
       
        public TextBox tweetMesh;


        public string url = "https://continuous-profile.herokuapp.com/tweets/";

        /// <summary>
        /// A model for the lower left corner of the frame to place when an image is detected.
        /// </summary>
        public GameObject Visualizer;
        public GameObject Plane;


        private int counterSleepy=0;
        private int counterAwake = 0;

        public float time = 0;
        public bool tracking = false;





        public void Start()
        {
            Invoke("readTweets",0);
            Plane.GetComponent<Renderer>().material.mainTexture = texture;



        }

        public void readTweets()
        {
            StartCoroutine(getTweets());
            Invoke("readTweets", 30);
        }

        IEnumerator getTweets()
        {

            //WWW www = new WWW("https://continuous-profile.herokuapp.com/tweets/trump");
            WWW www = new WWW("https://continuous-profile.herokuapp.com/tweets/" + (counter).ToString());
            yield return www;

            if (www.isDone)
            {
                if (www.error == null)
                {
                    //CON L'IMMAGINE
                    Plane.GetComponent<Renderer>().material.mainTexture = www.texture;
                    tweetMesh.changeText("");

                    //CON IL TESTO:
                    // string tweetsJson = www.text;
                    //Tweet tweet = JsonUtility.FromJson<List<Tweet>>(tweetsJson);

                    //tweetMesh.changeText(Image.DatabaseIndex.ToString()+ TweetJson.tweet[0].text);
                }
                else
                {
                    tweetMesh.changeText("");
                    //Plane.GetComponent<Renderer>().material.mainTexture = texture;
                }
            }
        }



        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {



            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                Visualizer.SetActive(false);
 

                return;
            }

            if (Image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
            {
                counterAwake++;
                if (!tracking) { tracking = true; time = Time.time; }
            }
            else if (Image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose) {
                if (counterAwake > 10) { counterSleepy++;
                    if (tracking) { tracking = false; time = Time.time; }

                }

            }

            //tweetMesh.changeText(Image.DatabaseIndex.ToString());
            tweetMesh.changeText(counter.ToString()+"\n -Full: "+ counterAwake.ToString()+",\n -Last:"+counterSleepy.ToString()+"\n Trigger time:"+time.ToString());
            //tweetMesh.changeText(Image.DatabaseIndex.ToString() + TweetJson.tweet[0].text);

            float halfWidth = Image.ExtentX / 3;
                float halfHeight = Image.ExtentZ / 4;
                Visualizer.transform.localPosition = (halfWidth * Vector3.zero) + (halfHeight * Vector3.zero);


            Visualizer.SetActive(true);
          

        }
    }
}
