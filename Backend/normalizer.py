import os
import sys
import numpy as np
import tensorflow as tf
from keras.models import Sequential
from keras.layers.core import Dense

import statistics as st
import math

model = Sequential()
model.add(Dense(64, input_dim=4, use_bias=True, activation='relu'))
model.add(Dense(128, use_bias=True, activation='relu'))
model.add(Dense(128, use_bias=True, activation='relu'))
model.add(Dense(64, use_bias=True, activation='relu'))
model.add(Dense(64, use_bias=True, activation='relu'))
model.add(Dense(3, use_bias=True, activation='sigmoid'))

sport = "Weightlifting"

checkpoint_path = sport + "/cp.ckpt"
checkpoint_dir = os.path.dirname(checkpoint_path)

# Create a callback that saves the model's weights
cp_callback = tf.keras.callbacks.ModelCheckpoint(filepath=checkpoint_path,
                                                 save_weights_only=True,
                                                 verbose=1)

model.load_weights(checkpoint_path)

#model.compile(loss='mean_squared_error',
#              optimizer='adam',
#              metrics=['mean_absolute_error'])

#model.fit(training_data, target_data, batch_size = 100, epochs=5000, verbose=2, shuffle = True, callbacks=[cp_callback])

#print(model.predict(training_data))

nationality = sys.argv[1]

goldPN = 0
goldM = 0
median = []
with open("data/" + sport + "_Gold.txt", "r") as f:
    f.readline()
    while True:
        line = f.readline()
        
        # if line is empty
        # end of file is reached
        if not line:
            break
        
        # Get next line from file
        frame = line.split(",")
        if (float(frame[1]) > 0):
            median.append(float(frame[1]))
        if (float(frame[1]) > goldM):
            goldM = float(frame[1])
        
        if (frame[0] == nationality):
            goldPN = float(frame[1])
    
median.sort()
goldM = math.sqrt(st.pvariance(median))

silverPN = 0
silverM = 0
median = [] 
with open("data/" + sport + "_Silver.txt", "r") as f:
    f.readline()
    while True:
        line = f.readline()
        
        # if line is empty
        # end of file is reached
        if not line:
            break
        
        # Get next line from file
        frame = line.split(",")
        if (float(frame[1]) > 0):
            median.append(float(frame[1]))
        if (float(frame[1]) > silverM):
            silverM = float(frame[1])
        
        if (frame[0] == nationality):
            silverPN = float(frame[1])
            break
     
        
median.sort()
silverM = math.sqrt(st.pvariance(median))

bronzePN = 0 
bronzeM = 0
median = [] 
with open("data/" + sport + "_Bronze.txt", "r") as f:
    f.readline()
    while True:
        line = f.readline()
        
        # if line is empty
        # end of file is reached
        if not line:
            break
        
        # Get next line from file
        frame = line.split(",")
        if (float(frame[1]) > 0):
            median.append(float(frame[1]))
        if (float(frame[1]) > bronzeM):
            bronzeM = float(frame[1])
        
        if (frame[0] == nationality):
            bronzePN = float(frame[1])
            break
     
        
median.sort()
bronzeM = math.sqrt(st.pvariance(median))
            
def clamp(value, min = 0, max = 0.99):
    if (value < min):
        value = min
    elif (value > max):
        value = max
    return value
            
response = ""
            
age = float(sys.argv[2])
weight = float(sys.argv[3])
height = float(sys.argv[4])
sex = float(sys.argv[5])
            
data = model.predict([[19,50,160,0]])[0]
response += str(data) + "\n"
            
gold = data[0]
if (goldPN >= goldM):
    gold = gold * (1 + goldPN - goldM)
    
silver = data[1]
if (silverPN >= silverM):
    silver = silver * (1 + silverPN - silverM)
    
bronze = data[1]
if (bronzePN >= bronzeM):
    bronze = bronze * (1 + bronzePN - bronzeM)
            
finalized = [clamp(gold), clamp(silver), clamp(bronze)]
response += str(finalized) + "\n"

if (len(sys.argv) == 12):
    d = 1
    with open("data/" + sport + "_data.txt", "r") as f:
        f.readline()
        data = f.readline()
        frame = data.split(",")
        
        d = float(frame[0])

    goldB = float(sys.argv[6])
    goldN = float(sys.argv[7])
    silverB = float(sys.argv[8])
    silverN = float(sys.argv[9])
    bronzeB = float(sys.argv[10])
    bronzeN = float(sys.argv[11])

    finalized = [(d * finalized[0] + (goldB - 0.5) * math.pow(goldN, 1/4)) / d, (d * finalized[0] + (silverB - 0.5) * math.pow(silverN, 1/4)) / d, (d * finalized[0] + (bronzeB - 0.5) * math.pow(bronzeN, 1/4)) / d]
    response += str(finalized) + "\n"

with open("results.txt", "w") as f:
    f.write(response)