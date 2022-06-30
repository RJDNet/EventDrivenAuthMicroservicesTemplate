import styled from 'styled-components';

interface IButtonProps {
    disable?: boolean;
}

export const Button = styled.button<IButtonProps>((props: IButtonProps) => ({
    width: '250px',
    margin: '5px',
    padding: '10px 20px 10px 20px',  
    borderRadius: '5px',
    fontSize: '20px',
    border: 'none',
    cursor: 'pointer',
    backgroundColor: props.disable ? 'red' : '',
    color: props.disable  ? 'white' : '',
    opacity: props.disable ? '15%' : ''
}));
