function ready() {
    if (document.readyState == 'complete') {
        Webcam.set({
            width: 320,
            height: 240,
            image_format: 'jpeg',
            jpeg_quality: 90
        });
        try {
            Webcam.attach('#camera');
        } catch (e) {
            alert(e);
        }
    }
}

function stopCamera() {
    Webcam.reset();
}

async function take_snapshot() {
    return new Promise((resolve, reject) => {
        Webcam.snap(function (data_uri) {
            resolve(data_uri);
        });
    });
}