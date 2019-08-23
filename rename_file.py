import os
import sys


def rename(path):
    i = 1
    for name in os.listdir(path):
        post = name.split('.')[-1]
        os.rename(os.path.join(path, name), os.path.join(path, str(i) + '.' + post))
        i += 1


def main(argv):
    rename(argv[1])
    
 
if __name__ == "__main__":
    main(sys.argv)
