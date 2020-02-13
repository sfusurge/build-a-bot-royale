var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Express' });
});

router.get('/test', function(req, res) {
  res.send("test api call worked!");
});

router.get('/socket_test', function(req, res) {
  res.render('socket_test');
});

module.exports = router;
