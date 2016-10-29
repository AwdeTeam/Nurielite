import math
import tensorflow as tf

types =
[
	"relu",     #tf.nn.relu(features) Rectified Linear
	"relu6",    #tf.nn.relu6(features)
	"crelu",    #tf.nn.crelu(features)
	"elu",      #tf.nn.elu(features)
	"softplus", #tf.nn.softplus(features)
	"softsign", #tf.nn.softsign(features)
	"dropout",  #tf.nn.dropout(x, keep_prob)
	"bias_add", #tf.nn.bias_add(value, bias)
	"sigmoid",  #tf.sigmoid(x)
	"tanh"      #tf.tanh(x)
]


def makeNet(dex, features, name):
	if(dex == 0):
		return tf.nn.relu(features, name)
	else if(dex == 1):
		return tf.nn.relu6(features, name)
	else if(dex == 2):
		return tf.nn.crelu(features, name)
	else if(dex == 3):
		return tf.nn.elu(fearures, name)
	else if(dex == 4):
		return tf.nn.softplus(features, name)
	else if(dex == 5):
		return tf.nn.softsign(features, name)
	else if(dex == 6):
		return None #this one is weird, don't use for now
	else if(dex == 7):
		return tf.nn.sigmoid(features)
	else if(dex == 8):
		return tf.nn.tanh(features)
	return None
