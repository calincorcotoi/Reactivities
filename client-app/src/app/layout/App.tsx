import { Fragment } from "react";
import { Container } from "semantic-ui-react";

import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";
import { Outlet, useLocation } from "react-router-dom";
import HomePage from "../../features/activities/home/HomePage";
import { ToastContainer } from "react-toastify";

function App() {
  const location = useLocation();

  return (
    <Fragment>
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      {location.pathname === "/" ? (
        <HomePage />
      ) : (
        <Fragment>
          <NavBar></NavBar>
          <Container style={{ marginTop: "7em" }}>
            {/* An <Outlet> renders whatever child route is currently active,
          so you can think about this <Outlet> as a placeholder for
          the child routes we defined above. */}
            <Outlet />
          </Container>
        </Fragment>
      )}
    </Fragment>
  );
}

export default observer(App);
