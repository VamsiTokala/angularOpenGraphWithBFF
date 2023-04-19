(function () {
    if (sessionStorage.redirect) {
      const redirect = sessionStorage.redirect;
      delete sessionStorage.redirect;
      history.replaceState(null, null, redirect);
    }
  })();
  