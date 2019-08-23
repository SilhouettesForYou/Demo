#-*- coding: UTF-8 -*-  
 
import os
import sys
from PIL import Image, ImageSequence
 
def analyse_image(path):
    im = Image.open(path)

    iter = ImageSequence.Iterator(im)

    index = 1
    if not os.path.exists(path[:-4]):
        os.path.mkdir(path[:-4])
    for frame in iter:
        print('image %d: mode %s , size %s' % (index, frame.mode, frame.size))
        frame.save('./' + path[:-4] + '/' + path[:-4] + '-%d' + '.png' % index)
        index += 1

 

 
 
 
def main(argv):
    analyse_image(argv[1])
    
 
if __name__ == "__main__":
    main(sys.argv)