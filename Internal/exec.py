import os
import shutil

workingDirectory = os.path.dirname(os.path.realpath(__file__))

source = workingDirectory
destination = workingDirectory + '\..\\bin\\x64\Release\Internal'

def copy(src, dest, ignore=None):
	if os.path.isdir(src):
		if not os.path.isdir(dest):
			os.makedirs(dest)
		files = os.listdir(src)
		if ignore is not None:
			ignored = ignore(src, files)
		else:
			ignored = set()
		for f in files:
			if f not in ignored:
				copy(os.path.join(src, f), os.path.join(dest, f), ignore)
	else:
		print('Copying ' + src + '...', end='')
		shutil.copyfile(src, dest)
		print('done!')

copy(workingDirectory, destination)
os.chdir(destination + '\..\\')
os.system(destination + '\..\d4lilah.exe')