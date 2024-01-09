import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Container, Header, Segment, Image } from "semantic-ui-react";
import { store } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {
  const { userStore, modalStore } = store;
  return (
    <Segment inverted textAlign="center" vertical className="masthead">
      <Container text>
        <Header as="h1" inverted>
          <Image size="massive" src="/assets/logo.png" alt="logo" style={{ marginBottom: 12 }} />
          Reactivities
        </Header>
        {userStore.isLoggedIn ? (
          <>
            <Button as={Link} to="/activities" size="huge" inverted>
              Go To Activities!
            </Button>
          </>
        ) : (
          <>
            <Header as="h2" inverted content="Welcome to Reactivities" />
            <Button onClick={() => modalStore.openModal(<LoginForm />)} size="huge" inverted>
              Login!
            </Button>
            <Button onClick={() => modalStore.openModal(<RegisterForm />)} size="huge" inverted>
              Register!
            </Button>
          </>
        )}
      </Container>
    </Segment>
  );
});
