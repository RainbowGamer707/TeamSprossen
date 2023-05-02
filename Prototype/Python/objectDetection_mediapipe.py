import time
import mediapipe as mp #GOOGLE'S MEDIAPIPE
import cv2 #OPENCV FOR LIVESTREAM CAPTURE
import numpy as np
from mediapipe.tasks import python

"""IMPORTANT:
For Livestream analysis, you will need to download the Machine Learning Model to your local device and then reference the full file path both here and
for the BaseOptions parameter in the ObjectDetectorOptions class. The file path will differ between users, so ensure you have changed this before
running the script.
For this script, we are using the recommended ML model (EfficientDet-Lite0) in float16 mode. 
https://developers.google.com/mediapipe/solutions/vision/object_detector/index#models
"""
model_path = '/.../Python/Model/efficientdet_lite0_fp16.tflite'

BaseOptions = mp.tasks.BaseOptions
#DetectionResult = mp.tasks.components.containers.detections.DetectionResult
ObjectDetector = mp.tasks.vision.ObjectDetector
ObjectDetectorOptions = mp.tasks.vision.ObjectDetectorOptions
VisionRunningMode = mp.tasks.vision.RunningMode

MARGIN = 10  # pixels
ROW_SIZE = 10  # pixels
FONT_SIZE = 1
FONT_THICKNESS = 1
TEXT_COLOR = (255, 0, 0)  # red

mp_drawing = mp.solutions.drawing_utils

def print_result(result, output_image, timestamp_ms: int):
    print('detection result: {}'.format(result))

options = ObjectDetectorOptions(
    base_options=BaseOptions(model_asset_path='/.../Python/Model/efficientdet_lite0_fp16.tflite'),
    running_mode=VisionRunningMode.LIVE_STREAM,
    max_results=5,
    result_callback=print_result,
    category_allowlist=['person'],
    score_threshold=0.6
    )

cap = cv2.VideoCapture(0)

with ObjectDetector.create_from_options(options) as detector:
    while cap.isOpened():
        success, frame = cap.read()
        if not success:
            print("Ignoring empty camera frame.")
            continue
        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        image = np.array(frame)
        timestamp_ms = int(time.time() * 1000)

        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=image)
        detector.detect_async(mp_image, timestamp_ms)
        
        cv2.imshow('Object Detection', frame)
        
        if cv2.waitKey(5) & 0xFF == ord('q'):
            break

cap.release()
cv2.destroyAllWindows()

#REFERENCES
#https://developers.google.com/mediapipe/solutions/vision/object_detector/python
#https://www.geeksforgeeks.org/python-opencv-capture-video-from-camera/
#https://colab.research.google.com/github/googlesamples/mediapipe/blob/main/examples/object_detection/python/object_detector.ipynb#scrollTo=H4aPO-hvbw3r&uniqifier=1
#Troubleshooting aided by ChatGPT: https://chat.openai.com



