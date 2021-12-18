const express = require('express');
var morgan = require('morgan');
const path = require('path');
const port = process.env.PORT || 8082;
const app = express();

// serve static assets normally
app.use(express.static(__dirname + '/ManagerWebClient/'));
app.use(morgan('combined'));

// handle every other route with index.html, which will contain
// a script tag to your application's JavaScript file(s).
app.get('*', function (request, response) {
  response.sendFile(path.resolve(__dirname, 'ManagerWebClient/index.html'));
});

app.listen(port);
console.log("server started on port " + port);