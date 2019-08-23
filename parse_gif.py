#-*- coding: UTF-8 -*-  
 
import os
import sys
from PIL import Image, ImageSequence
 
def analyse_image(path):
    im = Image.open(path)
    _, name = os.path.split(path)
    
    iter = ImageSequence.Iterator(im)

    split_dir = path[:-4]
    index = 1
    if not os.path.exists(split_dir):
        os.mkdir(split_dir)
    for frame in iter:
        print('image %d: mode %s , size %s' % (index, frame.mode, frame.size))
        frame.save(split_dir + '/' + name[:-3] + '-' + str(index) + '.png')
        index += 1
 
 
def main(argv):
    analyse_image(argv[1])
    
 
if __name__ == "__main__":
    main(sys.argv)