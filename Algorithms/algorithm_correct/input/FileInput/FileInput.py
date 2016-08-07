class PyAlgorithmInterface():

	algOptions = {
		"fname" : "'C:\\\\'",
		"comments" : "'#'",
		"delimiter" : "','",
		"skip_header" : "0",
		"skip_footer" : "0",
		"converters" : "None",
		"missing_values" : "None",
		"filling_values" : "None",
		"usecols" : "None",
		"excludelist" : "None",
		"deletechars" : "None",
		"replace_space" : "'_'",
		"autostrip" : "False",
		"case_sensitive" : "False",
		"defaultfmt" : "'f%i'",
		"unpack" : "False",
		"usemask" : "False",
		"loose" : "True",
		"invalid_raise" : "True",
		"XML" : "<options>"+
		"<option pythonkey='fname' guitype='file_chooser' label='Path to File' description='File or filename to read. If the filename extension is gz or bz2, the file is first decompressed' default='\"C:\\\\\"' />" +
		"<option pythonkey='comments' guitype='string_box' label='Comment Identifier' description='The character used to indicate the start of a comment. All the characters occurring on a line after a comment are discarded.' default='#' />" +
		"<option pythonkey='delimiter' guitype='string_box' label='String Delimeter' description='The string used to separate values. An integer or sequence of integers can also be provided as width(s) of each field.' default=',' />" +
		"<option pythonkey='skip_header' guitype='text_box' label='Skip Header Rows' description='The number of lines to skip at the beginning of the file.' default='0' />" +
		"<option pythonkey='skip_footer' guitype='text_box' label='Skip Footer Rows' description='The number of lines to skip at the end of the file.' default='0' />" +
		"<option pythonkey='converters' guitype='text_box' label='Converter Functions' description='The set of functions that convert the data of a column to a value.' default='None' />" +
		"<option pythonkey='missing_values' guitype='text_box' label='Missing Values' description='The set of strings corresponding to missing data.' default='None' />" +
		"<option pythonkey='filling_values' guitype='text_box' label='Filling Values' description='The set of values to be used as default when the data are missing.' default='None' />" +
		"<option pythonkey='usecols' guitype='text_box' label='Use Columns' description='Which columns to read.' default='None' />" +
		"<option pythonkey='excludelist' guitype='text_box' label='Excluded Names' description='A list of names to exclude. Excluded names are appended an underscore' default='None' />" +
		"<option pythonkey='deletechars' guitype='text_box' label='Delete Characters' description='A string combining invalid characters that must be deleted from the names.' default='None' />" +
		"<option pythonkey='replace_space' guitype='string_box' label='Replace Space' description='Character(s) used in replacement of white spaces in the variables names. ' default='_' />" +
		"<option pythonkey='autostrip' guitype='check_box' label='Auto-Strip' description='Whether to automatically strip white spaces from the variables.' default='False' />" +
		"<option pythonkey='case_sensitive' guitype='check_box' label='Case Sensitive' description='If True, field names are case sensitive. If False, field names are converted to upper case. ' default='False' />" +
		"<option pythonkey='defaultfmt' guitype='text_box' label='Default Field Names' description='A format used to define default field names.' default='None' />" +
		"<option pythonkey='unpack' guitype='check_box' label='Unpack' description='If True, the returned array is transposed, so that arguments may be unpacked.' default='False' />" +
		"<option pythonkey='usemask' guitype='check_box' label='Use Mask' description='If True, return a masked array. If False, return a regular array.' default='False' />" +
		"<option pythonkey='loose' guitype='check_box' label='Loose' description='If True, do not raise errors for invalid values.' default='True' />" +
		"<option pythonkey='invalid_raise' guitype='check_box' label='Invalid Raise' description='If True, an exception is raised if an inconsistency is detected in the number of columns. If False, a warning is emitted and the offending lines are skipped.' default='False' />"
		"</options>"
		}

	metaData = {
		"Name" : "File Input",
		"Creator" : "Benjamin Croisdale",
		"Version" : "1.1.0",
		"Default" : "<default><inputs></inputs><output>raw file input</output></default>"
		}

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions

	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		code = """
#importer = FileInputInput.FileInput()
OUT_DATA = numpy.genfromtxt(fname={FNAME}, comments={COMMENTS}, delimiter={DELIMITER}, skip_header={SKIP_HEADER}, skip_footer={SKIP_FOOTER}, converters={CONVERTERS}, missing_values={MISSING_VALUES}, filling_values={FILLING_VALUES}, usecols={USECOLS}, excludelist={EXCLUDELIST}, deletechars={DELETECHARS}, replace_space={REPLACE_SPACE}, autostrip={AUTOSTRIP}, case_sensitive={CASE_SENSITIVE}, defaultfmt={DEFAULTFMT}, unpack={UNPACK}, usemask={USEMASK}, loose={LOOSE}, invalid_raise={INVALID_RAISE})
		"""
		return code.format(FNAME=self.algOptions["fname"], COMMENTS=self.algOptions["comments"], DELIMITER=self.algOptions["delimiter"], SKIP_HEADER=self.algOptions["skip_header"], SKIP_FOOTER=self.algOptions["skip_footer"], CONVERTERS=self.algOptions["converters"], MISSING_VALUES=self.algOptions["missing_values"], FILLING_VALUES=self.algOptions["filling_values"], USECOLS=self.algOptions["usecols"], EXCLUDELIST=self.algOptions["excludelist"], DELETECHARS=self.algOptions["deletechars"], REPLACE_SPACE=self.algOptions["replace_space"], AUTOSTRIP=self.algOptions["autostrip"], CASE_SENSITIVE=self.algOptions["case_sensitive"], DEFAULTFMT=self.algOptions["defaultfmt"], UNPACK=self.algOptions["unpack"], USEMASK=self.algOptions["usemask"], LOOSE=self.algOptions["loose"], INVALID_RAISE=self.algOptions["invalid_raise"])

	def generateCodeLibraries(self):
		f = open("FileInput_class.py")
		FileInputLibrary = f.read()
		libraries = { "FileInputInput":FileInputLibrary, "numpy":"" };
		return libraries
