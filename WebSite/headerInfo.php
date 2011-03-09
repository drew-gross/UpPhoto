<?php
	function outputHeaders($titleAppend, $otherHeaders = "")
	{
echo <<<HERE
<title>$titleAppend - UpPhoto: Fast and simple Facebook Photo uploading</title>
<!-- <link type="text/css" rel="stylesheet" href="styles/reset.css" />
<link type="text/css" rel="stylesheet" href="styles/960.css" />
<link type="text/css" rel="stylesheet" href="styles/text.css" /> -->
<link type="text/css" rel="stylesheet" href="styles/styles.css" />
<link rel="shortcut icon" href="favicon.ico" />
$otherHeaders
HERE;
	}
?>
