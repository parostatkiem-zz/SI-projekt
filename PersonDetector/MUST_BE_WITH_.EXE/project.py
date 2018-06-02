from sklearn.neural_network import MLPClassifier
import os

import sys

os.system("echo off")
#print("elo")
file = open("user_probes.data")
data = []
label = []

for line in file:
    if(line.rstrip()==""):
        continue
    tmp = line.rstrip().split(' ')
    label.append(tmp[6])
    data.append([float(x) for x in tmp[0:6]])


#print(data[0],label[0])



tab = [[float(x) for x in sys.argv[1:]]]
#print(tab)
mlp =MLPClassifier(hidden_layer_sizes=(10,10,10), max_iter=1000)
mlp.fit(data, label)
os.system("cls")
print(mlp.predict(tab)[0])

