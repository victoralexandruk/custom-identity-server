<template lang="html">
  <nav class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0 shadow">
    <router-link to="/" class="navbar-brand col-md-3 col-lg-2 mr-0 px-3 py-2">
      <img src="images/logo.svg" height="32" alt="logo" />
    </router-link>
    <button class="navbar-toggler position-absolute d-md-none collapsed" type="button" data-toggle="collapse" data-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="d-none d-md-flex">
      <span class="navbar-text"><i class="icon-user"></i> {{myUser.name}}</span>
      <ul class="navbar-nav px-3">
        <li class="nav-item text-nowrap">
          <a class="nav-link" href="#" @click.prevent="$logout()"><i class="icon-log-out"></i> {{$localizer('Logout')}}</a>
        </li>
      </ul>
    </div>
  </nav>
</template>
