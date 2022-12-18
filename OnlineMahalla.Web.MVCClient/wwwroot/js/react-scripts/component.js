const Link = ReactRouterDOM.Link;
const Route = ReactRouterDOM.Route;
const { Button, Table } = window['antd'];
const { useEffect } = window['React'];

const columns = [
    {
        title: 'Name',
        dataIndex: 'name',
    },
    {
        title: 'Chinese Score',
        dataIndex: 'chinese',
        sorter: {
            compare: (a, b) => a.chinese - b.chinese,
            multiple: 3,
        },
    },
    {
        title: 'Math Score',
        dataIndex: 'math',
        sorter: {
            compare: (a, b) => a.math - b.math,
            multiple: 2,
        },
    },
    {
        title: 'English Score',
        dataIndex: 'english',
        sorter: {
            compare: (a, b) => a.english - b.english,
            multiple: 1,
        },
    },
];

const data = [
    {
        key: '1',
        name: 'John Brown',
        chinese: 98,
        math: 60,
        english: 70,
    },
    {
        key: '2',
        name: 'Jim Green',
        chinese: 98,
        math: 66,
        english: 89,
    },
    {
        key: '3',
        name: 'Joe Black',
        chinese: 98,
        math: 90,
        english: 70,
    },
    {
        key: '4',
        name: 'Jim Red',
        chinese: 88,
        math: 99,
        english: 89,
    },
];

const Home = () => {
    useEffect(() => {
        axios.get("@Url.Action("GetList", "OrganizationsSettlementAccount")")
            .then(function (response) {
                // handle success
                console.log(response);
            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
    }, []);

    function onChange(pagination, filters, sorter, extra) {
        console.log('params', pagination, filters, sorter, extra);
    }
    return (
        <div>
            <h1>hello</h1>
            <Table
                columns={columns}
                dataSource={data}
                onChange={onChange}
                    bordered />
        </div>
    )
}
const Login = () => <h1>Login</h1>
const Register = () => <h1>Register</h1>

const App = () => (
    <div style={{ padding: "0 15px" }}>
        <Home />
    </div>
)

ReactDOM.render(<App />, document.querySelector('#root'));