exports.transform = function (model) {
  if (model.author) {
    var authorHtml = 
      '<div class="page-author text-secondary mb-4 p-2 border-bottom" style="font-size: 0.95rem;">' +
      '  <i class="bi bi-person-fill me-1"></i> <strong>Author:</strong> ' + model.author +
      '</div>';
      
    model.conceptual = authorHtml + model.conceptual;
  }
  return model;
}
