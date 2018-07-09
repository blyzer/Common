const sortJson = require('sort-json'),
  glob = require('glob');

var getDirectories = function (src, callback) {
  glob(src + '/**/*', callback);
};

getDirectories('src/assets/i18n', function (err, res) {
  if (err) {
    console.log('Error', err);
    return -1;
   }
  sortJson.overwrite(res);
});
