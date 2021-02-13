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
        self.setFixedSize(300, 50)
        self.setMouseTracking(True)
        self.setWindowFlag(Qt.WindowMinimizeButtonHint, False)
        self.setWindowFlag(Qt.FramelessWindowHint)
        self.setWindowFlag(Qt.Tool)
        self.setWindowFlag(Qt.WindowStaysOnTopHint)
        self.setAttribute(Qt.WA_ShowWithoutActivating)
        #self.setWindowOpacity(0.8)
        self.setWindowOpacity(1.0)

        self.move(QDesktopWidget().availableGeometry().width() - self.geometry().width(), 0)
        self.label_1 = QLabel(sys.argv[2], self) 
        self.label_1.resize(300, 12)
        self.label_1.move(7, 5) 
        self.label_1.setFont(QFont('Arial', 12)) 

        self.label_2 = QLabel(sys.argv[3], self) 
        self.label_2.resize(300, 20)
        self.label_2.move(7, 20) 
        self.label_2.setFont(QFont('Arial', 9)) 

        self.show()
        threading.Thread(target=self.waitForExit, args=(), kwargs={}).start()

    def mouseMoveEvent(self, e):
        sys.exit()
        #self.label_2.setText(str(e.x()) + "   "  + str(e.y()))

    def waitForExit(self):
        time.sleep(int(sys.argv[1]))
        self.close()
        os._exit(1)

 
if len(sys.argv) != 4:
    sys.exit(print("wrong argv count"))

App = QApplication(sys.argv) 
window = Window() 
sys.exit(App.exec()) 
