from PyQt5.QtWidgets import * 
from PyQt5.QtGui import * 
from PyQt5.QtCore import Qt
import sys 
import threading
import time
import os


class Window(QMainWindow): 
    def __init__(self): 
        super().__init__() 
        self.setFixedSize(300, 60)
        self.setMouseTracking(True)
        self.setWindowFlag(Qt.WindowMinimizeButtonHint, False)
        self.setWindowFlag(Qt.FramelessWindowHint)
        self.setWindowFlag(Qt.Tool)
        self.setWindowFlag(Qt.WindowStaysOnTopHint)
        self.setAttribute(Qt.WA_ShowWithoutActivating)
        #self.setWindowOpacity(0.8)
        self.setWindowOpacity(1.0)

        self.move(QDesktopWidget().availableGeometry().width() - self.geometry().width(), 0 + int(sys.argv[1]))
        self.label_1 = QLabel(sys.argv[3], self) 
        self.label_1.setAlignment(Qt.AlignTop);
        self.label_1.resize(290, 20)
        self.label_1.move(7, 5)
        self.label_1.setFont(QFont('Arial', 12)) 

        # body:
        self.label_2 = QLabel(sys.argv[4], self) 
        self.label_2.setAlignment(Qt.AlignTop);
        self.label_2.resize(290, 40)
        self.label_2.move(7, 25) 
        self.label_2.setFont(QFont('Arial', 9)) 

        self.show()
        time.sleep(1)
        threading.Thread(target=self.waitForExit, args=(), kwargs={}).start()

    def mouseMoveEvent(self, e):
        print("detected mouse movement, closing now")
        self.close()
        os._exit(1)
        #self.label_2.setText(str(e.x()) + "   "  + str(e.y()))

    def waitForExit(self):
        time.sleep(int(sys.argv[2]))
        print("waited 5 seconds, closing now")
        self.close()
        os._exit(1)

 
if len(sys.argv) != 5:
    sys.exit(print("wrong argv count\n" + "expected 4 arguments, got " + str(len(sys.argv))))

try:
    App = QApplication(sys.argv) 
    window = Window() 
    sys.exit(App.exec()) 
except:
   print(sys.exc_info()[0])