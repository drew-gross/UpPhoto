<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
<title>UpPhoto - Fast and simple Facebook Photo uploading.</title>
<link type="text/css" rel="stylesheet" href="styles.css" />
</head>
<body>
<form action="index.php" method="post"><input type="text" name="in1" />
<input type="submit" /></form>
<div id="Background">
<div id="Main">Up-Photo
<div id="Info"><?php
date_default_timezone_set("UTC");
echo "<p>UpPhoto is a fast and easy photo uploader for facebook. With UpPhoto, the time it
					 takes to upload photos to Facebook drops from minutes to seconds. Just Put your
					 photos in a special UpPhoto folder that is created when you install UpPhoto, and
					 they will automatically appear on Facebook. You can even tell UpPhoto to upload
					 all the photos from your digital camera whenever you plug it in, or from your webcam
					 whenever you take a photo, and you don't need to do a thing.</p>";
?>
<p>UpPhoto is not available yet, if you are interested, check back in a
week or two.</p>
</div>
</div>
<?php 
require("footer.php");
?>
</div>
</div>
</body>
</html>
