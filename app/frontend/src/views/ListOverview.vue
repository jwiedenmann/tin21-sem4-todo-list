<template>
  <!-- <div class="lists">
    <h3>Todos for me</h3>
    <div class="list-group">
      <div v-for="todo in content.todoDataList" v-bind:key="todo.id">
        <a :href="'/list/' + todo.id" class="list-group-item list-group-item-action flex-column align-items-start">
          <div class="d-flex w-100 justify-content-between">
            <h5 class="mb-1">{{ todo.title }}</h5>
            <small>{{ todo.creationDate }}</small>
          </div>
          <small>{{ todo.role }}</small>
        </a>
      </div>
    </div>
  </div> -->
  <div class="row flex-grow-1 p-5 h-75 mb-4 bg-body-tertiary border rounded-3">
    <div class="container-fluid">
      <h1 class="display-5 fw-bold">Custom jumbotron</h1>
      <p class="col-md-8 fs-4">
        Using a series of utilities, you can create this jumbotron, just
        like the one in previous versions of Bootstrap. Check out the
        examples below for how you can remix and restyle it to your
        liking.
      </p>
      <button class="btn btn-primary btn-lg" type="button">
        Example button
      </button>
    </div>
  </div>

  <footer class="row justify-content-center pt-1 text-body-secondary border-top">
    <p>
      &copy; 2023
    </p>
  </footer>
  <!-- <div class="row">
      <div class="col-md-12">

        <div class="card" id="chat3" style="border-radius: 15px;">
          <div class="card-body">

            <div class="row">
              <div class="col-md-6 col-lg-5 col-xl-4 mb-4 mb-md-0">

                <div class="p-3">

                  <div class="input-group rounded mb-3">
                    <input type="search" class="form-control rounded" placeholder="Search" aria-label="Search"
                      aria-describedby="search-addon" />
                    <span class="input-group-text border-0" id="search-addon">
                      <i class="fas fa-search"></i>
                    </span>
                  </div>

                  <div data-mdb-perfect-scrollbar="true" style="position: relative; height: 400px">
                    <ul class="list-unstyled mb-0">
                      <li class="p-2 border-bottom">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-success badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Marie Horwitz</p>
                              <p class="small text-muted">Hello, Are you there?</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">Just now</p>
                            <span class="badge bg-danger rounded-pill float-end">3</span>
                          </div>
                        </a>
                      </li>
                      <li class="p-2 border-bottom">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava2-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-warning badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Alexa Chung</p>
                              <p class="small text-muted">Lorem ipsum dolor sit.</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">5 mins ago</p>
                            <span class="badge bg-danger rounded-pill float-end">2</span>
                          </div>
                        </a>
                      </li>
                      <li class="p-2 border-bottom">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-success badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Danny McChain</p>
                              <p class="small text-muted">Lorem ipsum dolor sit.</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">Yesterday</p>
                          </div>
                        </a>
                      </li>
                      <li class="p-2 border-bottom">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava4-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-danger badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Ashley Olsen</p>
                              <p class="small text-muted">Lorem ipsum dolor sit.</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">Yesterday</p>
                          </div>
                        </a>
                      </li>
                      <li class="p-2 border-bottom">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava5-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-warning badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Kate Moss</p>
                              <p class="small text-muted">Lorem ipsum dolor sit.</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">Yesterday</p>
                          </div>
                        </a>
                      </li>
                      <li class="p-2">
                        <a href="#!" class="d-flex justify-content-between">
                          <div class="d-flex flex-row">
                            <div>
                              <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
                                alt="avatar" class="d-flex align-self-center me-3" width="60">
                              <span class="badge bg-success badge-dot"></span>
                            </div>
                            <div class="pt-1">
                              <p class="fw-bold mb-0">Ben Smith</p>
                              <p class="small text-muted">Lorem ipsum dolor sit.</p>
                            </div>
                          </div>
                          <div class="pt-1">
                            <p class="small text-muted mb-1">Yesterday</p>
                          </div>
                        </a>
                      </li>
                    </ul>
                  </div>

                </div>

              </div>

              <div class="col-md-6 col-lg-7 col-xl-8">

                <div class="pt-3 pe-3" data-mdb-perfect-scrollbar="true" style="position: relative; height: 400px">

                  <div class="d-flex flex-row justify-content-start">
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                    <div>
                      <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">Lorem ipsum
                        dolor
                        sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et
                        dolore
                        magna aliqua.</p>
                      <p class="small ms-3 mb-3 rounded-3 text-muted float-end">12:00 PM | Aug 13</p>
                    </div>
                  </div>

                  <div class="d-flex flex-row justify-content-end">
                    <div>
                      <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">Ut enim ad minim veniam,
                        quis
                        nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                      <p class="small me-3 mb-3 rounded-3 text-muted">12:00 PM | Aug 13</p>
                    </div>
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                  </div>

                  <div class="d-flex flex-row justify-content-start">
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                    <div>
                      <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">Duis aute
                        irure
                        dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
                      </p>
                      <p class="small ms-3 mb-3 rounded-3 text-muted float-end">12:00 PM | Aug 13</p>
                    </div>
                  </div>

                  <div class="d-flex flex-row justify-content-end">
                    <div>
                      <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">Excepteur sint occaecat
                        cupidatat
                        non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                      <p class="small me-3 mb-3 rounded-3 text-muted">12:00 PM | Aug 13</p>
                    </div>
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                  </div>

                  <div class="d-flex flex-row justify-content-start">
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                    <div>
                      <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">Sed ut
                        perspiciatis
                        unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam
                        rem
                        aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae
                        dicta
                        sunt explicabo.</p>
                      <p class="small ms-3 mb-3 rounded-3 text-muted float-end">12:00 PM | Aug 13</p>
                    </div>
                  </div>

                  <div class="d-flex flex-row justify-content-end">
                    <div>
                      <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">Nemo enim ipsam
                        voluptatem quia
                        voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos
                        qui
                        ratione voluptatem sequi nesciunt.</p>
                      <p class="small me-3 mb-3 rounded-3 text-muted">12:00 PM | Aug 13</p>
                    </div>
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                  </div>

                  <div class="d-flex flex-row justify-content-start">
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                    <div>
                      <p class="small p-2 ms-3 mb-1 rounded-3" style="background-color: #f5f6f7;">Neque porro
                        quisquam
                        est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non
                        numquam
                        eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.</p>
                      <p class="small ms-3 mb-3 rounded-3 text-muted float-end">12:00 PM | Aug 13</p>
                    </div>
                  </div>

                  <div class="d-flex flex-row justify-content-end">
                    <div>
                      <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary">Ut enim ad minima veniam,
                        quis
                        nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea
                        commodi
                        consequatur?</p>
                      <p class="small me-3 mb-3 rounded-3 text-muted">12:00 PM | Aug 13</p>
                    </div>
                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                      alt="avatar 1" style="width: 45px; height: 100%;">
                  </div>

                </div>

                <div class="text-muted d-flex justify-content-start align-items-center pe-3 pt-3 mt-2">
                  <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp" alt="avatar 3"
                    style="width: 40px; height: 100%;">
                  <input type="text" class="form-control form-control-lg" id="exampleFormControlInput2"
                    placeholder="Type message">
                  <a class="ms-1 text-muted" href="#!"><i class="fas fa-paperclip"></i></a>
                  <a class="ms-3 text-muted" href="#!"><i class="fas fa-smile"></i></a>
                  <a class="ms-3" href="#!"><i class="fas fa-paper-plane"></i></a>
                </div>

              </div>
            </div>

          </div>
        </div>

      </div>
    </div> -->
</template>

<style scoped>
#chat3 .form-control {
  border-color: transparent;
}

#chat3 .form-control:focus {
  border-color: transparent;
  box-shadow: inset 0px 0px 0px 1px transparent;
}

.badge-dot {
  border-radius: 50%;
  height: 10px;
  width: 10px;
  margin-left: 2.9rem;
  margin-top: -.75rem;
}
</style>

<script>
export default {
  data() {
    return {
      content: {
        todoDataList: [
          {
            "id": 1,
            "title": "Meine erste Todo",
            "creationDate": "2022-04-01",
            "role": "admin"
          },
          {
            "id": 2,
            "title": "Noch eine Todo",
            "creationDate": "2022-07-01",
            "role": "admin"
          },
          {
            "id": 3,
            "title": "Hausaufgaben",
            "creationDate": "2023-04-01",
            "role": "user"
          },
          {
            "id": 4,
            "title": "Bucket List",
            "creationDate": "2023-09-10",
            "role": "admin"
          },
          {
            "id": 5,
            "title": "Einkaufsliste",
            "creationDate": "2012-04-01",
            "role": "readOnly"
          }
        ]
      }
    }

  }
}
</script>