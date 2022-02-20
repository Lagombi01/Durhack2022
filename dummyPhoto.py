from os import path
from PIL import Image
from io import BytesIO
    

if __name__ == "__main__":
    script_dir = path.dirname(path.realpath(__file__)) #<-- absolute dir the script is in
    rel_path = "Olympic_rings_without_rims.svg.png"
    abs_file_path = path.join(script_dir, rel_path)
    print(abs_file_path)
    # with open(rel_path,"rb") as f:
    #     d = f.read()
    # with open("bytes.txt","w+") as ff:
    #     ff.write(d)
    with open("bytes.txt","rb") as f:
        d = f.read()
    
    tempFile = BytesIO()
    
    img = Image.open(BytesIO(d))
    img.convert('RGB')
    
    img.save("pic1.png",format="PNG")
    # with open("pic1.png","w+") as ff:
    #     ff.write(d)