<?php
function navlink($target, $name, $selected){
  if($selected){
    echo "<li><a href=\"$target\" class=\"selected\">$name</a></li>";
  } else {
    echo "<li><a href=\"$target\">$name</a></li>";
  }
}

function navlinks($current){
  $links = array("/" => "Home", "contact.php" => "Contact Us");
  echo "<ul class=\"navigation\">";
  foreach($links as $link => $name){
    navlink($link, $name, $name == $current);
  }
  echo "</ul>";
}
?>
