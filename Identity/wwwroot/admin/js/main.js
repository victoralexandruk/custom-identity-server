const store = {
  myUser: null,
  localizerStrings: {}
};

// Create an instance of Notyf
var notyf = new Notyf({
  position: {
    x: 'right',
    y: 'top'
  },
  dismissible: true
});

httpVueLoader.httpRequest = function (url) {
	const version = '20210829.00';
	return new Promise(function(resolve, reject) {
		$.get(url + '?v=' + version).done(resolve).fail(reject);
	});
};

Vue.prototype.$moment = moment;
Vue.prototype.$localizer = function (key) {
  return store.localizerStrings[key] || key;
};
Vue.prototype.$localizer.loadFile = function (lang) {
  $.get(`lang/${lang}.json`).done(function (strings) {
    store.localizerStrings = strings;
  }).fail(function () {
    store.localizerStrings = {};
  });
};
Vue.prototype.$logout = auth.redirectToLogout;

Vue.mixin({
  data() {
    return store;
  }
});

Vue.component('topbar', httpVueLoader('components/Topbar.vue'));
Vue.component('sidebar', httpVueLoader('components/Sidebar.vue'));

const router = new VueRouter({
  routes: [
    { path: '/', component: httpVueLoader('pages/BaseAuthenticated.vue'), meta: { requiresAuth: true }, children: [
      { path: '', component: httpVueLoader('pages/Home.vue'), meta: { requiresAuth: true } },
      { path: 'user', component: httpVueLoader('pages/user/List.vue') },
      { path: 'user/:id', component: httpVueLoader('pages/user/Edit.vue') },
      { path: 'client', component: httpVueLoader('pages/client/List.vue') },
      { path: 'client/:id', component: httpVueLoader('pages/client/Edit.vue') },
    ] },
  ],
  linkExactActiveClass: 'active'
});

router.beforeEach(async (to, from, next) => {
  const user = await api.getMyUser();
  if (to.matched.some(record => record.meta.requiresAuth) && !user) {
    auth.redirectToLogin(to.fullPath);
	} else {
		next();
	}
});

var app = new Vue({
  el: '#app',
  router: router,
  created() {
    this.$localizer.loadFile('pt-br');
    api.getMyUser().then(user => store.myUser = user);
  }
});
