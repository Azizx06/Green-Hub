// Dynamic Images and Texts for Intro Section
// Arrays for both texts and images
const introImages = [
    '/images/Green_Initiative.jpg',
    '/images/RecycleExample1.jpeg',
    '/images/Beaches_Preservation_Initiative.png'
];

const introTexts = [
    'Your Guide to Sustainable Living',
    'Together for a Greener Tomorrow',
    'Join Us in Making a Difference'
];

// the index to go through the array
let introIndex = 0;

// function to cycle through the images
function updateIntroSection() {

    const introImage = document.getElementById('introImage');
    const introText = document.getElementById('introText');

    // Fade out image and text (HTML DOM - Changing CSS)
    introImage.style.opacity = 0;
    introText.style.opacity = 0;

    // After fade-out, update the image source and text, then fade back in
    setTimeout(() => {

        // Set image and text based on their respective index indicator
        introImage.src = introImages[introIndex];
        introText.textContent = introTexts[introIndex];

        // Fade back in image and text
        introImage.style.opacity = 1;
        introText.style.opacity = 1;

        /* Move to the next index, looping back if needed.. 
        both of introImages or introTexts can be used, both have same length */
        introIndex = (introIndex + 1) % introImages.length;

    }, 500); // 500ms timeout to match CSS transition duration
}

// Change the intro image and text every 5 seconds, to loop updateIntroSection()
setInterval(updateIntroSection, 5000);





/* function notifyMe(daysBefore) {

    if (!SignInManager.IsSignedIn(User)) {
        alert("Please log in to set notifications.");
        return;
    }

    // If the user is logged in, proceed with notification
    alert('Notification set successfully! You will be notified ' + daysBefore + ' days before the event.');
} */






