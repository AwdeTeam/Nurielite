# Nurielite
## Version 0.8 (pre-alpha)
Nurielite is a machine-learning graphical interface that allows for model persistence and sharing.  The concept is based around the observation that many machine learning algorithms are cheap to use, but expensive to train, so it would be useful to create a library of generic (and, hopefully, _general_) pre-trained algorithms.

Additionally, because of how machine learning can be complicated to approach even though it usually follows a standard style, we suspected that a GUI would significantly reduce prototype-time, even for experienced programmers and especially for relative novices.

The GUI is written in C# using WPF and it generates python code that is theoretically runnable anywhere (although a python 2.7 environment with Numpy and Theano must be available).

Right now, we have (ZERO!) actual algorithms implemented and adding more is one of our top priorities.
