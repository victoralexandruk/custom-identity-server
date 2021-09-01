<template lang="html">
  <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
    <div class="sidebar-sticky pt-0">

      <div class="d-md-none border-bottom">
        <div class="d-flex align-items-end p-3 sidebar-pattern-bg text-white" style="height: 100px;">
          <h5 class="font-weight-light m-0"><i class="icon-user"></i> {{myUser.name}}</h5>
        </div>
        <ul class="nav flex-column py-2">
          <li class="nav-item">
            <a class="nav-link" href="#" @click.prevent="$logout()">
              <i class="icon-log-out"></i>
              {{$localizer('Logout')}}
            </a>
          </li>
        </ul>
      </div>

      <ul class="nav flex-column py-2">
        <li class="nav-item">
          <router-link to="/" class="nav-link">
            <i class="icon-home"></i>
            {{$localizer('Dashboard')}}
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/user" class="nav-link">
            <i class="icon-users"></i>
            {{$localizer('Users')}}
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/client" class="nav-link">
            <i class="icon-globe"></i>
            {{$localizer('Clients')}}
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/api" class="nav-link">
            <i class="icon-box"></i>
            {{$localizer('APIs')}}
          </router-link>
        </li>
      </ul>
    </div>
  </nav>
</template>
