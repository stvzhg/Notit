#!/usr/bin/python
import os
import re
import json
import sys


def asmOfOne(dct, fname):
	file = open(fname, 'w')
	flg = 1
	for i in range(len(dct)):
		if flg == 1:
			file.write(str(dct[i]['offset']) + '\n')
		file.write(dct[i]['content'] + ' ')
		flg = 0
		if i < len(dct) - 1 and dct[i + 1]['offset'] - dct[i]['offset'] > 1000:
			#if i<len(dct)-2:
			file.write('\n')
			flg = 1
	file.close()

#def min(argv):
	#sys.stdout.write('asdfasdf')
	#return 'asdf'



def main(argv):
	#for i in argv:
	#	print i
	name=[]
	path = []
	tmppath = []
	for arg in argv[1:-1]:
		path.append(arg)
		name.append(arg[10:-5])
		tmppath.append(arg[10:-5]+'tmp')
	mid = argv[-1]
	fp = []
	personWord = []

	for i in path:
		fp.append(open(i))
	for t in fp:
		txtrd = t.read()
		#txtrdr=txtrdr[1:-1]
		#print type(txtrd)
		#txtrd.replace(r'\\','')
		#txtrd=re.sub(r'\\','',txtrdr)
		#print(txtrd)
		#print(json.loads(txtrd))
		personWord.append(json.loads(txtrd))
		t.close()
	for count in range(len(argv) - 2):
		asmOfOne(personWord[count]['document'], tmppath[count])
	output = open(mid, 'w')
	file = []
	time = []
	seq = 0
	c = 0
	for i in tmppath:
		file.append(open(i, 'r'))
		tm = file[seq].readline()
		seq += 1
		if tm:
			time.append(int(tm))
		else:
			time.append(999999999)
	while min(time) != 999999999:
		c = time.index(min(time))
		output.write(name[c] + '\nTime: ' + str(time[c]) + '\n' + file[c].readline()+'\n')
		tm = file[c].readline()
		if len(tm):
			time[c] = int(tm)
		else:
			time[c] = 999999999
	output.close()
	rd = open(mid)
	res = rd.read()
	rd.close()
	#os.system(r'rm -rf '+mid)
	for count in range(len(argv)-2):
		os.system(r'rm -rf '+tmppath[count])
		os.system(r'rm -rf '+path[count])
	os.system('pwd')
	#return res
	
if __name__ == "__main__":
	main(sys.argv)
	#\os.system('pwd')
	#ret = 'ass'
	#print ret
	#sys.stdout.write('asdfa')
	#return ret
