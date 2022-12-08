import os
import numpy as np
from PIL import Image
import sewar
import matplotlib.pyplot as plt

DATA_DIR = "ImagesCopy"

def compare_images(image1, image2):
	psnr = sewar.psnr(image1, image2)
	ssim = sewar.ssim(image1, image2)[0]
	vif = sewar.vifp(image1, image2)

	print("Comparison statistics")
	print(f"PSNR = {psnr}")
	print(f"SSIM = {ssim}")
	print(f"VIF = {vif}")

	return psnr, ssim, vif

def compare_for_scene(scene_type, compare_pair, compare_title=None):
	if compare_title is None:
		compare_title = compare_pair
	print(f"Comparing {compare_pair[0]} and {compare_pair[1]} view for scene type - {scene_type}")
	image1_filename = scene_type + "_" + compare_pair[0] + "_view.jpg"
	image2_filename = scene_type + "_" + compare_pair[1] + "_view.jpg"

	img1 = Image.open(os.path.join(DATA_DIR, image1_filename))
	img2 = Image.open(os.path.join(DATA_DIR, image2_filename))
	
	plt.subplot(1, 2, 1)
	plt.imshow(img1)
	plt.title(compare_title[0])
	plt.subplot(1, 2, 2)
	plt.imshow(img2)
	plt.title(compare_title[1])
	plt.show()

	metrics = compare_images(np.asarray(img1), np.asarray(img2))
	return metrics

def plot_metric_pairs(metrics_pair, scene_pair, view_pair, metrics, colors):
	fig = plt.figure(figsize=(10, 5))
	fig.canvas.set_window_title(view_pair[0] + " and " + view_pair[1])
	num_metrics = len(metrics_pair[0])
	for i in range(num_metrics):
		ax = plt.subplot(1, num_metrics, i+1)
		plt.plot([metrics_pair[j][i] for j in range(len(metrics_pair))], marker='o',color=colors[i])
		plt.xticks([j for j in range(len(scene_pair))])
		ax.set_xticklabels(scene_pair)
		plt.title(metrics[i])
	plt.show()

def main():
	metrics_user_same = []
	metrics_user_same.append(compare_for_scene("base", ("user_spine", "same_spine"), ("Base User View", "Base Unauthorized view")))
	metrics_user_same.append(compare_for_scene("ar", ("user_spine", "same_spine"), ("AR User View", "AR Unauthorized view")))
	metrics_user_same.append(compare_for_scene("ar_haptics", ("user_spine", "same_spine"), ("AR ETHD User View", "AR ETHD Unauthorized view")))
	plot_metric_pairs(metrics_user_same, ('Base', 'AR', 'AR ETHD'), ("User View", "Unauthorized view"), ('PSNR', 'SSIM', 'VIF'), ['r', 'g', 'b'])
	
if __name__ == '__main__':
	main()