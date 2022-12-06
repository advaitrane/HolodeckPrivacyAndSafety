import os
import numpy as np
from PIL import Image
import sewar
import matplotlib.pyplot as plt

def compare_images(image1, image2):
	psnr = sewar.psnr(image1, image2)
	ssim = sewar.ssim(image1, image2)[0]
	vif = sewar.vifp(image1, image2)

	print("Comparison statistics")
	print(f"PSNR = {psnr}")
	print(f"SSIM = {ssim}")
	print(f"VIF = {vif}")

	return psnr, ssim, vif

def main():
	image1_filename = "ImagesCopy/ar_haptics_user_xxx_view.jpg"
	image2_filename = "ImagesCopy/ar_haptics_same_xxx_view.jpg"

	img1 = Image.open(image1_filename)
	img2 = Image.open(image2_filename)
	
	plt.subplot(1, 2, 1)
	plt.imshow(img1)
	plt.title("Original")
	plt.subplot(1, 2, 2)
	plt.imshow(img2)
	plt.title("Silhouette")
	plt.show()

	metrics = compare_images(np.asarray(img1), np.asarray(img2))

if __name__ == '__main__':
	main()